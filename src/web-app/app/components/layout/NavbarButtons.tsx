import { VisualCompleteItemData } from '@/app/data/VisualCompleteItemData';
import { Button, ButtonGroup, Link } from '@heroui/react';
import { useTranslations } from 'next-intl';
import CurrentUser from './CurrentUser';

interface PagesNavbarProps {
    items: VisualCompleteItemData[];
}

const LangSwitcherTranslationSection = 'PagesNavbar';

export default function NavbarButtons({ items }: PagesNavbarProps) {
    const t = useTranslations(LangSwitcherTranslationSection);
    return (
        <ButtonGroup>
            <CurrentUser />
            {items.map((item) => (
                <Button
                    variant="bordered"
                    as={Link}
                    href={item.url}
                    key={item.id}
                    endContent={<item.icon size={16} />}
                >
                    {t(item.name)}
                </Button>
            ))}
        </ButtonGroup>
    );
}
