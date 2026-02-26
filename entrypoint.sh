#!/bin/sh
# entrypoint.sh - Set up SSH tunnel to cPanel MySQL before starting the .NET app

if [ -n "$SSH_PRIVATE_KEY" ]; then
  echo "[entrypoint] Setting up SSH tunnel to cPanel MySQL..."

  # Write SSH private key
  mkdir -p /root/.ssh
  echo "$SSH_PRIVATE_KEY" | base64 -d > /root/.ssh/id_ed25519
  chmod 600 /root/.ssh/id_ed25519

  # Disable strict host key checking for cPanel server
  cat > /root/.ssh/config << 'SSHEOF'
Host 103.9.76.10
  User iskrwaod
  IdentityFile /root/.ssh/id_ed25519
  StrictHostKeyChecking no
  ServerAliveInterval 30
  ServerAliveCountMax 3
SSHEOF

  # Start SSH tunnel: forward local 3306 to remote MySQL
  ssh -f -N -o ExitOnForwardFailure=yes \
      -L 3306:127.0.0.1:3306 \
      iskrwaod@103.9.76.10 2>&1

  if [ $? -eq 0 ]; then
    echo "[entrypoint] SSH tunnel established: localhost:3306 -> cPanel MySQL"
    sleep 1
  else
    echo "[entrypoint] WARNING: SSH tunnel failed"
  fi
fi

exec "$@"
