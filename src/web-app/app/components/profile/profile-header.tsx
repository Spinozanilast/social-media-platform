import { Button, Image, Snippet, Spinner } from '@heroui/react';
import ImageTooltip from '~/components/common/image-tooltip';
import { UserRoundPen } from 'lucide-react';
import React, { useMemo } from 'react';
import { isStringEmptyOrNull } from '~/utils/helpers';

type ProfileHeaderProps = {
    firstName: string;
    lastName: string;
    userName: string;
    imageUrl: string;
    isOwner: boolean;
    onModalOpen: () => void;

    translations: {
        edit: string;
    };
};

export default function ProfileHeader({
    firstName,
    lastName,
    userName,
    imageUrl,
    isOwner,
    onModalOpen,
    translations,
}: ProfileHeaderProps) {
    const namePart = useMemo(() => {
        if (isStringEmptyOrNull(firstName) && isStringEmptyOrNull(lastName)) {
            return (
                <div className="flex flex-col gap-1">
                    <h2 className="text-2xl font-bold">{userName}</h2>
                </div>
            );
        }
        return (
            <div className="flex flex-col gap-1">
                <div className="flex flex-row justify-between gap-4">
                    <h2 className="text-2xl font-bold">
                        {firstName} {lastName}
                    </h2>
                    {isOwner && (
                        <Button
                            size="sm"
                            className="p-4 mx-2 mb-1"
                            color="primary"
                            onPress={onModalOpen}
                            endContent={<UserRoundPen />}
                        >
                            {translations.edit}
                        </Button>
                    )}
                </div>
                <Snippet
                    className="w-full bg-default-100"
                    variant="shadow"
                    symbol="@"
                    tooltipProps={{
                        content: 'Copy username',
                        disableAnimation: true,
                        placement: 'right',
                        closeDelay: 0,
                    }}
                >
                    {userName}
                </Snippet>
            </div>
        );
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [firstName, lastName]);

    return (
        <div className="flex flex-row gap-5 items-center justify-center">
            {imageUrl === '/profile.svg' ? (
                <Image
                    className="rounded-md shadow shadow-accent-orange p-2"
                    src={imageUrl}
                    alt="Profile image"
                />
            ) : (
                <ImageTooltip
                    width={300}
                    imageUrl={imageUrl}
                    toolTipProps={{
                        placement: 'right-start',
                        delay: 700,
                    }}
                    imageProps={{ isZoomed: true }}
                >
                    {imageUrl ? (
                        <Image
                            className="rounded-md min-w-16 cursor-pointer hover:shadow-lg"
                            src={imageUrl}
                            alt="Profile image"
                        />
                    ) : (
                        <Spinner color="primary" size="md" />
                    )}
                </ImageTooltip>
            )}
            {namePart}
        </div>
    );
}
