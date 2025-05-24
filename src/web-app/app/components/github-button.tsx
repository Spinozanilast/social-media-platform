import { Button, Divider } from '@heroui/react';
import { SiGithub } from '@icons-pack/react-simple-icons';
import AuthService from '~/api/auth/service';

type GithubAuthButtonProps = {
    withDividerAfter?: boolean;
    className?: string;
};

export default function GithubAuthButton({
    className,
    withDividerAfter = true,
}: GithubAuthButtonProps) {
    const handleClick = async () => {
        const response = await AuthService.githubSignIn();
        console.log(response);
    };

    return (
        <div className="w-full flex-col flex items-center justify-center">
            <Button variant="faded" onPress={handleClick} className={className}>
                Sign in with Github <SiGithub />
            </Button>
            {withDividerAfter && (
                <div className="w-full flex flex-row justify-center mt-4 items-center text-foreground-500">
                    <Divider className="shrink" />
                    <p className="text-sm text-center mx-2">OR</p>
                    <Divider className="shrink" />
                </div>
            )}
        </div>
    );
}
