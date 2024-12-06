import React from 'react';
import { GoEye, GoEyeClosed } from 'react-icons/go';

const ShowPasswordButton = ({
    isVisible,
    onClick,
}: {
    isVisible: boolean;
    onClick: () => void;
}) => {
    return (
        <button type="button" onClick={onClick}>
            {isVisible ? (
                <GoEyeClosed className="pointer-events-none text-2xl text-default-400" />
            ) : (
                <GoEye className="pointer-events-none text-2xl text-default-400" />
            )}
        </button>
    );
};

export default ShowPasswordButton;
