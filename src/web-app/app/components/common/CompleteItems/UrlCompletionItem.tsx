import { AutocompleteItem, Button, Link } from '@nextui-org/react';
import UrlCompleteItemData from '@data/CompleteItemsData/UrlCompleteItemData';
import { SiCurl } from 'react-icons/si';

import { redirect } from 'next/navigation';

const createUrlCompletionItem = (urlCompleteItem: UrlCompleteItemData) => {
    return (
        <AutocompleteItem
            key={urlCompleteItem.id}
            value={urlCompleteItem.name}
            textValue={urlCompleteItem.name}
        >
            <div className="flex items-center justify-between">
                <div className="flex gap-2 items-center">
                    <SiCurl />
                    <div className="flex flex-col">
                        <span className="text-small">
                            {urlCompleteItem.name}
                        </span>
                        <span className="text-tiny text-default-400">
                            {urlCompleteItem.url}
                        </span>
                    </div>
                </div>
            </div>
        </AutocompleteItem>
    );
};

export default createUrlCompletionItem;
