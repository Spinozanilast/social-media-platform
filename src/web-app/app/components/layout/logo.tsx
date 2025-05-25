'use client';

import OnDisplayAnimatedSpan from '~/components/common/animated-span';
import '@fontsource/share-tech-mono';
import { useEffect, useMemo, useState } from 'react';
import Link from 'next/link';
import { useTheme } from 'next-themes';
import { cn } from '@heroui/react';

type LogoProps = {
    animationType: 'backward' | 'forward';
    className?: string;
};

export default function Logo({
    animationType,
    className: textClassName,
}: LogoProps) {
    const [mounted, setMounted] = useState(false);
    const theme = useTheme();
    const [isHovered, setIsHovered] = useState(false);

    useEffect(() => {
        setMounted(true);
    }, []);

    const handlePointerEnter = () => {
        setIsHovered(true);
    };

    const handlePointerLeave = () => {
        setIsHovered(false);
    };

    const logoClassName = useMemo(() => {
        return cn(
            'w-full group logo text-accent-orange text-3xl',
            textClassName,
            mounted && theme.theme === 'light' ? 'logo-dark' : 'logo-light'
        );
    }, [theme.theme, textClassName, mounted]);

    const animatedSpanStyle =
        animationType === 'forward' ? 'group-hover:inline hidden' : 'inline';

    return (
        <Link
            className="w-32"
            style={{ fontFamily: 'Share Tech Mono' }}
            onMouseEnter={handlePointerEnter}
            onMouseLeave={handlePointerLeave}
            href="/"
        >
            <p
                className={logoClassName}
            >
                P
                <OnDisplayAnimatedSpan
                    animationType={animationType}
                    classname={animatedSpanStyle}
                    duration={2}
                    isHovered={isHovered}
                >
                    latfor
                </OnDisplayAnimatedSpan>
                m
            </p>
        </Link>
    );
}
