import type { Metadata } from 'next';
import './globals.css';
import { getLocale, getMessages } from 'next-intl/server';
import { NextIntlClientProvider } from 'next-intl';
import { cookies, headers } from 'next/headers';

export const metadata: Metadata = {
    title: 'Platform',
    description: 'Social media platform',
};

const StandardLocale = 'en-US';

export default async function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html>
            <body className="min-h-[100vh]">
                <NextIntlClientProvider>{children}</NextIntlClientProvider>
            </body>
        </html>
    );
}
