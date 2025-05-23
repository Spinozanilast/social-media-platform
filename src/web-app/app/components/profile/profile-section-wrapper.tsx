export default function ProfileSectionWrapper({
    children,
}: Readonly<{ children: React.ReactNode }>) {
    return (
        <div className="flex flex-col gap-2 w-full bg-content2 text-content2-foreground p-2 rounded-lg">
            {children}
        </div>
    );
}
