import Image from "next/image";
import "@fontsource/share-tech-mono";

export default function Home() {
    return (
        <div className="mx-page-part mt-page-part grid columns-3">
            <aside className="page-column"></aside>
            <main className="page-column"></main>
            <aside
                style={{ fontFamily: "Share Tech Mono" }}
                className="page-column flex flex-row gap-8 items-center"
            >
                <button>
                    <img className=" rounded-full" alt="profileImage" />
                </button>
                <h2 className="text-lg">Budchanin Vadim Alexandrovich</h2>
                <h2 className="text-base text-gray-400">
                    @Budchanin Vadim Alexandrovich
                </h2>
            </aside>
        </div>
    );
}
