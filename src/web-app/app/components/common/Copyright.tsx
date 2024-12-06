import { Link } from '@nextui-org/react';

export default function Copyright() {
    return (
        <div className="w-full">
            <p className="text-center text-sm text-gray-300">
                Copyright © 2024{' '}
                <Link
                    isBlock
                    href="https://github.com/Spinozanilast/social-media-platform/"
                    className="text-accent-orange font-bold text-base"
                >
                    Platform
                </Link>
                . All rights reserved.
            </p>
        </div>
    );
}
