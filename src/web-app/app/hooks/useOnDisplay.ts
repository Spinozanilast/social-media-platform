/* eslint-disable react-hooks/exhaustive-deps */
import { RefObject, useEffect, useMemo, useState, useRef } from "react";

export default function useOnScreen(ref: RefObject<HTMLElement | null>) {
    const observerRef = useRef<IntersectionObserver | null>(null);
    const [isOnScreen, setIsOnScreen] = useState(false);

    useEffect(() => {
        observerRef.current = new IntersectionObserver(([entry]) =>
            setIsOnScreen(entry.isIntersecting)
        );
    }, []);

    useEffect(() => {
        observerRef.current?.observe(ref.current!);
        return () => {
            observerRef.current?.disconnect();
        };
    }, [ref]);

    return isOnScreen;
}
