'use client';
import React, { useEffect, useState, useRef } from 'react';

type AnimatedSpanProps = {
    children: string;
    animationType: 'backward' | 'forward';
    classname: string;
    duration: number;
    isHovered: boolean;
};

export default function OnDisplayAnimatedSpan({
    children,
    animationType = 'forward',
    classname,
    isHovered,
    duration = 2,
}: AnimatedSpanProps) {
    const spanRef = useRef<HTMLSpanElement>(null);
    const timers = useRef<NodeJS.Timeout[]>([]);
    const [currentText, setCurrentText] = useState(
        animationType === 'forward' ? '' : children
    );

    const singleCharacterAppearTime = (duration / children.length) * 1000;

    const handleAnimation = (direction: 'forward' | 'backward') => {
        let newText = '';
        if (direction === 'forward') {
            newText = '';
        } else {
            newText = children;
        }

        children.split('').map((char, index) => {
            const timer = setTimeout(() => {
                if (direction === 'forward') {
                    setCurrentText((prevText) => prevText + char);
                } else {
                    setCurrentText((prevText) =>
                        prevText.slice(0, prevText.length - 1)
                    );
                }
            }, singleCharacterAppearTime * index);
            timers.current.push(timer);
        });
    };

    useEffect(() => {
        if (isHovered) {
            handleAnimation(animationType);
        } else {
            animationType === 'forward'
                ? setCurrentText('')
                : setCurrentText(children);
            timers.current.forEach((timer) => clearTimeout(timer));
            timers.current = [];
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isHovered, children, singleCharacterAppearTime, animationType]);

    return (
        <span ref={spanRef} className={`${classname}`}>
            {currentText.split('').map((char, index) => (
                <span className="animated-span" key={index}>
                    {char}
                </span>
            ))}
        </span>
    );
}
