#!/bin/sh
# entrypoint.sh - Register container outbound IP with cPanel to whitelist MySQL access

# Get our outbound IP
MY_IP=$(curl -s --max-time 8 https://api.ipify.org 2>/dev/null || echo "unknown")
echo "[entrypoint] Container outbound IP: $MY_IP"

if [ "$MY_IP" != "unknown" ] && [ -n "$MY_IP" ]; then
  echo "[entrypoint] Registering IP $MY_IP with cPanel for MySQL access..."
  RESULT=$(curl -s --max-time 15 "https://intra-africa.com/add_render_ip.php?k=renderaccess2026x" 2>/dev/null || echo "registration failed")
  echo "[entrypoint] Registration result: $RESULT"
else
  echo "[entrypoint] Could not determine outbound IP, skipping registration"
fi

exec "$@"
