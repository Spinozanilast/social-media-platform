import React from "react";
import "@fontsource/share-tech-mono";
import OnDisplayAnimatedSpan from "../utils/OnDisplayAnimatedSpan";

export default function Logo() {
    return (
        <p
            className="group logo text-accent-orange text-3xl"
            style={{ fontFamily: "Share Tech Mono" }}
        >
            P
            <OnDisplayAnimatedSpan
                classname="hidden group-hover:inline"
                duration={2}
            >
                latfor
            </OnDisplayAnimatedSpan>
            m
        </p>
    );
}
