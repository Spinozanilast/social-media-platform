type CopyrightProps = {
    mt?: number;
};

export default function Copyright({ mt }: CopyrightProps) {
    return (
        <div className={`w-full ${mt ? `mt-${mt}` : ''}`}>
            <p className="text-center text-sm share-tech-mono">
                Copyright © 2024{' '}
                <a
                    href="https://github.com/Spinozanilast/social-media-platform/"
                    className="text-accent-orange font-bold text-base"
                >
                    Platform
                </a>
                . All rights reserved.
            </p>
        </div>
    );
}
