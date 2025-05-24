export default function ProfileSectionWrapper({
    children,
}: Readonly<{ children: React.ReactNode }>) {
    return (
        <div className="flex flex-col gap-2 w-full bg-content1 text-content1-foreground dark:bg-content2 dark:text-content2-foreground p-2 rounded-lg">
            {children}
        </div>
    );
}
