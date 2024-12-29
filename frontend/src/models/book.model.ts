export interface Book {
  id: number; // Optional since it may not exist during creation
  title: string;
  author: string;
  isbn: string;
  publicationDate: string;
}
