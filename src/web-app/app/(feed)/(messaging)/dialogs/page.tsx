import { MessagesSquare } from "lucide-react";

export default function Page() {
    return (
        <div className="page-column content-center flex gap-5 font-thin max-w-full sm:flex-col md:flex-row">
            <MessagesSquare className='left-up-element' />
        </div>
    );
}