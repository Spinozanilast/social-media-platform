import {
    SiTelegram,
    SiVk,
    SiGithub,
    SiWhatsapp,
} from '@icons-pack/react-simple-icons';

const linkStyles: Record<
    string,
    { borderColor: string; icon: JSX.Element | null }
> = {
    't.me': {
        borderColor: 'border-[#26A5E4]',
        icon: <SiTelegram className="social-link" color="#26A5E4" />,
    },
    'vk.com': {
        borderColor: 'border-[#0077FF]',
        icon: <SiVk className="social-link" color="#0077FF" />,
    },
    'github.com': {
        borderColor: 'border-[#181717]',
        icon: <SiGithub className="social-link" color="#181717" />,
    },
    'wa.me': {
        borderColor: 'border-[#25D366]',
        icon: <SiWhatsapp className="social-link" color="#25D366" />,
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
