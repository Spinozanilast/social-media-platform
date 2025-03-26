'use client';

import OnDisplayAnimatedSpan from '../special/OnDisplayAnimatedSpan';
import '@fontsource/share-tech-mono';
import { useState } from 'react';

type LogoProps = {
    animationType: 'backward' | 'forward';
    className: string;
};

export default function Logo({
    animationType,
    className: textClassName,
}: LogoProps) {
    const [isHovered, setIsHovered] = useState(false);

    const handlePointerEnter = () => {
        setIsHovered(true);
    };

    const handlePointerLeave = () => {
        setIsHovered(false);
    };

    const animatedSpanStyle =
        animationType === 'forward' ? 'group-hover:inline hidden' : 'inline';

    return (
        <div
            className="w-32"
            style={{ fontFamily: 'Share Tech Mono' }}
            onMouseEnter={handlePointerEnter}
            onMouseLeave={handlePointerLeave}
        >
            <p
                className={`w-full group logo text-accent-orange text-3xl + ${textClassName}`}
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
        </div>
    );
}
