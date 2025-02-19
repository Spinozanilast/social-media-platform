import createNextIntlPlugin from 'next-intl/plugin';
const withNextIntl = createNextIntlPlugin();

/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        remotePatterns: [
            {
                protocol: 'https',
                hostname: 'profile-images-bucket.s3.yandexcloud.net',
            },
            {
                protocol: 'https',
                hostname: "cdn.simpleicons.org",
                port: ''
            }
        ]
    }
};

export default withNextIntl(nextConfig);
