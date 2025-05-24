import '~/styles/globals.css';
import '~/styles/github-markdown.css';
import '~/styles/github-markdown-light.css';
import '~/styles/github-markdown-dark.css';

import HeroProvider from '~providers/hero-provider';
import React from 'react';

import type { Metadata } from 'next';
import { NextIntlClientProvider } from 'next-intl';
import { getLocale, getMessages } from 'next-intl/server';

export const metadata: Metadata = {
    title: 'Platform',
    description: 'Social media platform',
};

export default async function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    const locale = await getLocale();
    const messages = await getMessages();

    return (
        <html lang={locale} suppressHydrationWarning>
            <head>
                <script
                    dangerouslySetInnerHTML={{
                        __html: `
              (function() {
                var theme = localStorage.getItem('theme');
                var systemDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
                if (theme === 'dark' || (!theme && systemDark)) {
                  document.documentElement.classList.add('dark');
                } else {
                  document.documentElement.classList.remove('dark');
                }
              })();
            `,
                    }}
                />
                <script
                    async
                    src="https://unpkg.com/react-scan/dist/auto.global.js"
                />
            </head>
            <body>
                <NextIntlClientProvider messages={messages}>
                    <HeroProvider>{children}</HeroProvider>
                </NextIntlClientProvider>
            </body>
        </html>
    );
}
