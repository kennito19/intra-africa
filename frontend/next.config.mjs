/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        remotePatterns: [
            { protocol: 'https', hostname: 'placehold.jp' },
            { protocol: 'https', hostname: 'via.placeholder.com' },
            { protocol: 'https', hostname: 'img.freepik.com' },
            { protocol: 'https', hostname: 'encrypted-tbn0.gstatic.com' },
            { protocol: 'https', hostname: 'img.youtube.com' },
            { protocol: 'https', hostname: 'www.youtube.com' },
            { protocol: 'https', hostname: 'api.intra-africa.com' },
            { protocol: 'https', hostname: 'placehold.co' },
        ],
    },
};

export default nextConfig;