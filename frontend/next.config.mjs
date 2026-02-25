/** @type {import('next').NextConfig} */
const nextConfig = {
    distDir: 'build',
    images: {
        unoptimized: process.env.NODE_ENV !== 'production',
        remotePatterns: [
            { protocol: 'https', hostname: 'placehold.jp' },
            { protocol: 'https', hostname: 'via.placeholder.com' },
            { protocol: 'https', hostname: 'img.freepik.com' },
            { protocol: 'https', hostname: 'encrypted-tbn0.gstatic.com' },
            { protocol: 'https', hostname: 'img.youtube.com' },
            { protocol: 'https', hostname: 'www.youtube.com' },
            { protocol: 'https', hostname: 'api.intra-africa.com' },
            { protocol: 'https', hostname: 'placehold.co' },
            { protocol: 'http', hostname: 'localhost', port: '5010' },
            { protocol: 'http', hostname: 'localhost', port: '7246' },
            { protocol: 'http', hostname: '127.0.0.1', port: '5010' },
            { protocol: 'http', hostname: '127.0.0.1', port: '7246' },
        ],
    },
};

export default nextConfig;
