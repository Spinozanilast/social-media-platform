import { Tooltip, Image, ImageProps, TooltipProps } from '@nextui-org/react';

interface ImageTooltipProps {
    width: number;
    imageUrl: string;
    children: React.ReactNode;
    imageProps?: ImageProps;
    toolTipProps?: TooltipProps;
}

const ImageTooltip: React.FC<ImageTooltipProps> = ({
    width,
    imageUrl,
    children,
    imageProps,
    toolTipProps,
}) => {
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
};

export default ImageTooltip;
