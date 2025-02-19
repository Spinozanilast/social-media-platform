import React, { DOMAttributes } from 'react';
import { GoEye, GoEyeClosed } from 'react-icons/go';

const ShowPasswordButton = ({
    isVisible,
    onPress,
}: {
    isVisible: boolean;
    onPress: () => void;
}) => {
    return (
        <button type="button" onPress={onPress}>
            {isVisible ? (
                <GoEyeClosed className="pointer-events-none text-2xl text-default-400" />
            ) : (
                <GoEye className="pointer-events-none text-2xl text-default-400" />
            )}
        </button>
    );
};

export default ShowPasswordButton;
