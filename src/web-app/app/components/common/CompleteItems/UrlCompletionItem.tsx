import { AutocompleteItem, Button, Link, SelectItem } from '@nextui-org/react';
import CompleteItemData from '@/app/data/CompleteItemData';
import { SiCurl } from 'react-icons/si';

const createUrlCompletionItem = (urlCompleteItem: CompleteItemData) => {
    return (
        <SelectItem
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
        </SelectItem>
    );
};

export default createUrlCompletionItem;
