export default function RegisterPage({ params }: { params: { slug: string } }) {
    return (
        <div>
            <h1>Username: {params.slug}</h1>
            <section className="bg-background-secondary">
                <h1></h1>
            </section>
        </div>
    );
}
