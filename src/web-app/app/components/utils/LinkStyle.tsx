import { FaTelegramPlane, FaVk, FaGithub, FaWhatsapp } from 'react-icons/fa';

const linkStyles: Record<
    string,
    { borderColor: string; icon: JSX.Element | null }
> = {
    't.me': {
        borderColor: 'border-blue-500',
        icon: <FaTelegramPlane className="inline-block mr-2" />,
    },
    'vk.com': {
        borderColor: 'border-blue-600',
        icon: <FaVk className="inline-block mr-2" />,
    },
    'github.com': {
        borderColor: 'border-gray-800',
        icon: <FaGithub className="inline-block mr-2" />,
    },
    'wa.me': {
        borderColor: 'border-green-500',
        icon: <FaWhatsapp className="inline-block mr-2" />,
    },
    default: {
        borderColor: 'border-gray-200',
        icon: null,
    },
};

export const getLinkStyle = (url: string) => {
    try {
        const validUrl = url.startsWith('http') ? url : `https://${url}`;
        const domain = new URL(validUrl).hostname;
        const linkStyle = linkStyles[domain] || linkStyles.default;
        return { linkStyle, validUrl };
    } catch {
        const validUrl = url.startsWith('http') ? url : `https://${url}`;
        return { linkStyle: linkStyles.default, validUrl };
    }
};
