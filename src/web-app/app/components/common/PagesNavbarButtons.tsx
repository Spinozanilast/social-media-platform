import { VisualCompleteItemData } from '@/app/data/VisualCompleteItemData';
import { Button, ButtonGroup, Link } from '@nextui-org/react';
import { useTranslations } from 'next-intl';
import { TiMessages } from 'react-icons/ti';

interface PagesNavbarProps {
    items: VisualCompleteItemData[];
}

const LangSwitcherTranslationSection = 'PagesNavbar';

export default function PagesNavbarButtons({ items }: PagesNavbarProps) {
    const t = useTranslations(LangSwitcherTranslationSection);
    return (
        <ButtonGroup>
            {items.map((item) => (
                <Button
                    variant="bordered"
                    as={Link}
                    href={item.url}
                    key={item.id}
                    endContent={<item.icon />}
                >
                    {t(item.name)}
                </Button>
            ))}
        </ButtonGroup>
    );
}
