'use client';

export default function GlobalError({
    error,
    reset,
}: {
    error: Error & { digest?: string };
    reset: () => void;
}) {
    return (
        <html>
            <body className="w-full flex items-center justify-center">
                <h2>Something went wrong!</h2>
                <h2>{error.message}</h2>
                <button onClick={() => reset()}>Try again</button>
            </body>
        </html>
    );
}
