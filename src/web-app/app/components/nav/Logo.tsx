"use client";
import OnDisplayAnimatedSpan from "../utils/OnDisplayAnimatedSpan";
import "@fontsource/share-tech-mono";
import { useState } from "react";

type LogoProps = {
    animationType: "backward" | "forward";
};

export default function Logo({ animationType }: LogoProps) {
    const [isHovered, setIsHovered] = useState(false);

    const handlePointerEnter = () => {
        setIsHovered(true);
    };

    const handlePointerLeave = () => {
        setIsHovered(false);
    };

    const animatedSpanStyle =
        animationType === "forward" ? "group-hover:inline hidden" : "inline";

    return (
        <p
            className="group logo text-accent-orange text-3xl"
            style={{ fontFamily: "Share Tech Mono" }}
            onPointerEnter={handlePointerEnter}
            onPointerLeave={handlePointerLeave}
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
    );
}
