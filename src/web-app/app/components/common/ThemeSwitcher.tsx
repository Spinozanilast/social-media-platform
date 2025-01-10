'use client';

import { SwitchProps, useSwitch, VisuallyHidden } from '@nextui-org/react';
import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';
import { IoMoon, IoSunny } from 'react-icons/io5';

export function ThemeSwitcher(props: SwitchProps) {
    const [mounted, setMounted] = useState(false);
    const { theme, setTheme } = useTheme();

    const isLightThemeOn = theme === 'light';

    const { Component, slots, getBaseProps, getInputProps, getWrapperProps } =
        useSwitch(props);

    useEffect(() => {
        setMounted(true);
    }, []);

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
                        'utility-small-icon',
                        'flex items-center justify-center',
                        'px-0 mr-0',
                    ],
                })}
            >
                {isLightThemeOn ? <IoMoon /> : <IoSunny />}
            </div>
        </Component>
    );
}
