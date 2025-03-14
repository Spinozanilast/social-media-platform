import { Link } from "@heroui/react";

type CopyrightProps = {
    mt?: number;
};

export default function Copyright({ mt }: CopyrightProps) {
    return (
        <div className={`w-full ${mt ? `mt-${mt}` : ''}`}>
            <p className="text-center text-sm share-tech-mono">
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
