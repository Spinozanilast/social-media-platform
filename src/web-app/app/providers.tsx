import { NextUIProvider } from '@nextui-org/react';

export function Providers({ children }: { children: React.ReactNode }) {
    return (
        <NextUIProvider className="min-h-[100vh]">{children}</NextUIProvider>
    );
}
