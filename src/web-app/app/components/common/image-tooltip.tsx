import { Tooltip, Image, ImageProps, TooltipProps } from '@heroui/react';
import React from 'react';

interface ImageTooltipProps {
    width: number;
    imageUrl: string;
    children: React.ReactNode;
    imageProps?: ImageProps;
    toolTipProps?: TooltipProps;
}

export default function ImageTooltip({
    width,
    imageUrl,
    children,
    imageProps,
    toolTipProps,
}: ImageTooltipProps) {
    return (
        <Tooltip
            className="p-2"
            {...toolTipProps}
            content={
                <div className="flex flex-col gap-2">
                    <div className="text-center">{toolTipProps?.content}</div>
                    <Image
                        alt="profile image"
                        src={imageUrl}
                        width={width}
                        height="auto"
                        {...imageProps}
                    />
                </div>
            }
        >
            {children}
        </Tooltip>
    );
}
