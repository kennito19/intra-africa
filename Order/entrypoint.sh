#!/bin/sh
# entrypoint.sh - Set up SSH tunnel to cPanel MySQL before starting the .NET app

if [ -n "$SSH_PRIVATE_KEY" ]; then
  echo "[entrypoint] SSH_PRIVATE_KEY is set"

  # Write SSH private key
  mkdir -p /root/.ssh
  chmod 700 /root/.ssh
  printf '%s' "$SSH_PRIVATE_KEY" | base64 -d > /root/.ssh/id_ed25519
  chmod 600 /root/.ssh/id_ed25519

  KEY_BYTES=$(wc -c < /root/.ssh/id_ed25519)
  echo "[entrypoint] SSH key written: $KEY_BYTES bytes"

  if [ "$KEY_BYTES" -lt 100 ]; then
    echo "[entrypoint] ERROR: Key too small, base64 decode failed!"
    exec "$@"
  fi

  # Configure SSH host
  cat > /root/.ssh/config << 'SSHEOF'
Host cpanel
  HostName 103.9.76.10
  User iskrwaod
  Port 22
  IdentityFile /root/.ssh/id_ed25519
  StrictHostKeyChecking no
  UserKnownHostsFile /dev/null
  BatchMode yes
  ConnectTimeout 15
  ServerAliveInterval 30
  ServerAliveCountMax 3
SSHEOF
  chmod 600 /root/.ssh/config

  # Test if SSH port is reachable
  echo "[entrypoint] Testing connectivity to 103.9.76.10:22 ..."
  if nc -z -w10 103.9.76.10 22; then
    echo "[entrypoint] Port 22 is REACHABLE"

    # Start SSH tunnel with verbose output
    echo "[entrypoint] Starting SSH tunnel (local 3306 -> cPanel MySQL 127.0.0.1:3306)..."
    ssh -v -f -N \
        -o ExitOnForwardFailure=yes \
        -L 3306:127.0.0.1:3306 \
        cpanel 2>&1

    SSH_RESULT=$?
    echo "[entrypoint] SSH exit code: $SSH_RESULT"

    if [ $SSH_RESULT -eq 0 ]; then
      # Wait for the tunnel port to be ready
      MAX_WAIT=20
      i=0
      while [ $i -lt $MAX_WAIT ]; do
        if nc -z -w2 127.0.0.1 3306; then
          echo "[entrypoint] Tunnel READY: localhost:3306 accepting connections (attempt $i)"
          break
        fi
        i=$((i+1))
        echo "[entrypoint] Waiting for tunnel... attempt $i/$MAX_WAIT"
        sleep 1
      done

      if [ $i -eq $MAX_WAIT ]; then
        echo "[entrypoint] WARNING: localhost:3306 never became ready after $MAX_WAIT seconds"
      fi
    else
      echo "[entrypoint] SSH tunnel FAILED (exit code $SSH_RESULT)"
    fi
  else
    echo "[entrypoint] CRITICAL: Port 22 on 103.9.76.10 is NOT REACHABLE from this container"
    echo "[entrypoint] SSH tunnel cannot be established - MySQL will be unavailable"
  fi
else
  echo "[entrypoint] SSH_PRIVATE_KEY not set, skipping SSH tunnel setup"
fi

exec "$@"
