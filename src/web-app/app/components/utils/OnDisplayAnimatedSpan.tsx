"use client";
import React, { useEffect, useState, useRef } from "react";

type AnimatedSpanProps = {
    children: string;
    animationType: "backward" | "forward";
    classname: string;
    duration: number;
    isHovered: boolean;
};

export default function OnDisplayAnimatedSpan({
    children,
    animationType = "forward",
    classname,
    isHovered,
    duration = 2,
}: AnimatedSpanProps) {
    const spanRef = useRef<HTMLSpanElement>(null);
    const timers = useRef<NodeJS.Timeout[]>([]);
    const [currentText, setCurrentText] = useState(
        animationType === "forward" ? "" : children
    );

    const singleCharacterAppearTime = (duration / children.length) * 1000;

    const handleForwardAnimation = () => {
        setCurrentText("");
        children.split("").map((char, index) => {
            const timer = setTimeout(() => {
                setCurrentText((prevText) => prevText + char);
            }, singleCharacterAppearTime * index);
            timers.current.push(timer);
        });
    };

    const handleBackwardAnimation = () => {
        setCurrentText(children);
        children.split("").map((_char, index) => {
            const timer = setTimeout(() => {
                setCurrentText((prevText) =>
                    prevText.slice(0, prevText.length - 1)
                );
            }, singleCharacterAppearTime * index);
            timers.current.push(timer);
        });
    };

    useEffect(() => {
        if (isHovered) {
            if (animationType === "forward") {
                handleForwardAnimation();
            } else {
                handleBackwardAnimation();
            }
        } else {
            animationType === "forward"
                ? setCurrentText("")
                : setCurrentText(children);
            timers.current.forEach((timer) => clearTimeout(timer));
            timers.current = [];
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isHovered, children, singleCharacterAppearTime, animationType]);

    return (
        <span ref={spanRef} className={`${classname}`}>
            {currentText.split("").map((char, index) => (
                <span key={index}>{char}</span>
            ))}
        </span>
    );
}
