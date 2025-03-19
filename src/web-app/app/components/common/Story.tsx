// 'use client';
// import Story from '@/app/api/types/stories/stories';
// import {
//     Button,
//     Card,
//     CardBody,
//     CardHeader,
//     Chip,
//     Divider,
//     Dropdown,
//     DropdownItem,
//     DropdownMenu,
//     DropdownTrigger,
// } from '@heroui/react';
// import Markdown from 'react-markdown';
// import { format } from 'date-fns';
// import { Roboto } from 'next/font/google';
// import remarkGfm from 'remark-gfm';
// import remarkBreaks from 'remark-breaks';
// import { useCallback } from 'react';
// import StoriesService from '@/app/api/services/story';
// import { EllipsisVertical } from 'lucide-react';
//
// type StoryCardProps = {
//     story: Story;
//     isAuthenticated: boolean;
//     onDeleteStory: (storyId: number) => void;
// };
//
// const formatStoryDateStamp = (date: Date) => {
//     return format(date, 'dd-MM-yyyy HH:mm:ss');
// };
//
// const roboto = Roboto({
//     subsets: ['latin'],
//     weight: ['700'],
// });
//
// export const StoryCard: React.FC<StoryCardProps> = ({
//     story,
//     isAuthenticated,
//     onDeleteStory,
// }) => {
//     const markdownText = story.content.replace(/\\n/gi, '&nbsp;&nbsp;\n');
//     const handleDeleteStory = useCallback(async () => {
//         try {
//             await StoriesService.deleteStory(story.id);
//             onDeleteStory(story.id);
//         } catch (error) {
//             console.error(error);
//         }
//         // eslint-disable-next-line react-hooks/exhaustive-deps
//     }, [story.id]);
//
//     return (
//         <Card className="w-full" key={story.id}>
//             <CardHeader className="flex flex-col gap-2">
//                 <div className="flex flex-row w-full gap-2">
//                     <div className="flex flex-row justify-between w-full">
//                         <div className="flex flex-row gap-1">
//                             <p className="timestamp-pre-text">Created At</p>
//                             <p className="story-timestamp">
//                                 {formatStoryDateStamp(story.createdAt)}
//                             </p>
//                         </div>
//                         {story.updatedAt &&
//                             story.updatedAt !== story.createdAt && (
//                                 <div className="flex flex-row gap-1">
//                                     <p className="timestamp-pre-text">
//                                         Edited At
//                                     </p>
//                                     <p className="story-timestamp">
//                                         {formatStoryDateStamp(story.updatedAt)}
//                                     </p>
//                                 </div>
//                             )}
//                     </div>
//                     {isAuthenticated && (
//                         <Dropdown backdrop="blur">
//                             <DropdownTrigger>
//                                 <Button
//                                     className="h-full"
//                                     isIconOnly
//                                     variant="bordered"
//                                 >
//                                     <EllipsisVertical />
//                                 </Button>
//                             </DropdownTrigger>
//                             <DropdownMenu
//                                 aria-label="Static Actions"
//                                 variant="faded"
//                             >
//                                 <DropdownItem key="edit">
//                                     Edit Story
//                                 </DropdownItem>
//                                 <DropdownItem
//                                     key="delete"
//                                     className="text-danger"
//                                     color="danger"
//                                     onPress={handleDeleteStory}
//                                 >
//                                     Delete Story
//                                 </DropdownItem>
//                             </DropdownMenu>
//                         </Dropdown>
//                     )}
//                 </div>
//                 <div className="flex flex-row items-center gap-2">
//                     {story.tags.map((tag, index) => (
//                         <Chip color="primary" variant="shadow" key={index}>
//                             {tag}
//                         </Chip>
//                     ))}
//                 </div>
//                 <h1
//                     className={`text-xl text-accent-orange font-bold ${roboto.className}`}
//                 >
//                     {story.title}
//                 </h1>
//             </CardHeader>
//             <Divider />
//             <CardBody>
//                 <Markdown
//                     remarkPlugins={[remarkGfm, remarkBreaks]}
//                     className="markdown"
//                 >
//                     {markdownText}
//                 </Markdown>
//             </CardBody>
//         </Card>
//     );
// };
