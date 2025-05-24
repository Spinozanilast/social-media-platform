'use client';

import { SwitchProps, useSwitch, VisuallyHidden } from '@heroui/react';
import { Moon, Sun } from 'lucide-react';
import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';

export function ThemeSwitcher(props: SwitchProps) {
    const [mounted, setMounted] = useState(false);
    const { theme, setTheme } = useTheme();

    const isLightThemeOn = theme === 'light';

    const { Component, slots, getBaseProps, getInputProps, getWrapperProps } =
        useSwitch(props);

    useEffect(() => {
        setMounted(true);
    }, []);

    useEffect(() => {
        document.documentElement.setAttribute('data-theme', theme!);
    }, [theme]);

    if (!mounted) return null;

    return (
        <Component {...getBaseProps()}>
            <VisuallyHidden>
                <input {...getInputProps()} />
            </VisuallyHidden>
            <div
                onClick={() => setTheme(isLightThemeOn ? 'dark' : 'light')}
                {...getWrapperProps()}
                className={slots.wrapper({
                    class: [
                        'utility-large-icon',
                        'flex items-center justify-center',
                        'px-0 mr-0 rounded-lg',
                    ],
                })}
            >
                {isLightThemeOn ? <Moon /> : <Sun />}
            </div>
        </Component>
    );
}
