'use client';

import { HeroUIProvider } from '@heroui/react';
import { ThemeProvider as NextThemesProvider } from 'next-themes';
import React from 'react';

export default function HeroProvider({ children }: { children: React.ReactNode }) {
    return (
        <HeroUIProvider className="min-h-[100vh]">
            <NextThemesProvider attribute="class" defaultTheme="dark">
                {children}
            </NextThemesProvider>
        </HeroUIProvider>
    );
}
