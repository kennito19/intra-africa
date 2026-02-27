#!/bin/sh
# Register container outbound IP with cPanel to whitelist MySQL access
echo "[entrypoint] Registering container IP with cPanel for MySQL access..."
RESULT=$(curl -s --max-time 15 "http://intra-africa.com/add_render_ip.php?k=renderaccess2026x" 2>/dev/null || echo "registration failed")
echo "[entrypoint] Registration result: $RESULT"
exec "$@"
