import { Button, Link } from '@nextui-org/react';
import { redirect } from 'next/dist/server/api-utils';
import { FaFaceFrown } from 'react-icons/fa6';

export default function NotFound() {
    return (
        <main className="flex w-full h-full flex-col items-center justify-center gap-2">
            <FaFaceFrown className="w-10 text-accent-orange" />
            <h2 className="text-xl font-semibold">404 Not Found</h2>
            <p>User you are looking for does not exist.</p>
            <Button
                as={Link}
                href="/"
                className="mt-4 rounded-md bg-blue-500 px-4 py-2 text-sm text-white transition-colors hover:bg-blue-400"
            >
                Go Home
            </Button>
        </main>
    );
}
