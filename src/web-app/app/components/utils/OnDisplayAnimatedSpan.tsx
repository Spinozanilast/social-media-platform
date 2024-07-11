"use client";
import React, { useEffect, useState, useRef } from "react";
import useOnScreen from "../hooks/useOnDisplay";

type AnimatedSpanProps = {
    children: string;
    classname: string;
    duration: number;
};

export default function OnDisplayAnimatedSpan({
    children,
    classname,
    duration = 2,
}: AnimatedSpanProps) {
    const spanRef = useRef<HTMLSpanElement>(null);
    const timers = useRef([]);
    const [currentText, setCurrentText] = useState("");
    const isVisible = useOnScreen(spanRef);

    const singleCharacterAppearTime = (duration / children.length) * 1000;

    useEffect(() => {
        console.log(isVisible);
        if (isVisible) {
            setCurrentText("");
            children.split("").map((char, index) => {
                const timer = setTimeout(() => {
                    setCurrentText((prevText) => prevText + char);
                }, singleCharacterAppearTime * index);
                timers.current.push(timer);
            });
        } else {
            setCurrentText("");
            timers.current.forEach((timer) => clearTimeout(timer));
            timers.current = [];
        }
    }, [isVisible, children, singleCharacterAppearTime]);

    return (
        <span ref={spanRef} className={`${classname} group`}>
            {currentText.split("").map((char, index) => (
                <span key={index}>{char}</span>
            ))}
        </span>
    );
}
