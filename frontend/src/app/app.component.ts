import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model'; 
import { AsyncPipe } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, AsyncPipe, FormsModule, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  http = inject(HttpClient);

  // Form for adding and updating books
  booksForm = new FormGroup({
    id: new FormControl<number | null>(null), 
    title: new FormControl<string>(''),
    author: new FormControl<string>(''),
    isbn: new FormControl<string>(''),
    publicationDate: new FormControl<string>('')
  });

  books$ = this.getBooks(); 
  isUpdateMode = false; 

  // Handles form submission for Add or Update
  onFormSubmit() {
    const bookRequest = {
      title: this.booksForm.value.title,
      author: this.booksForm.value.author,
      isbn: this.booksForm.value.isbn,
      publicationDate: this.booksForm.value.publicationDate
    };
  
    if (this.isUpdateMode && this.booksForm.value.id !== null) {
      // Update existing book
      this.http.put(`https://localhost:7275/api/Book/${this.booksForm.value.id}`, {
        ...bookRequest,
        id: this.booksForm.value.id // Include id for updates
      }).subscribe({
        next: () => {
          console.log('Book updated successfully');
          this.books$ = this.getBooks();
          this.resetForm();
        }
      });
    } else {
      // Add new book
      this.http.post('https://localhost:7275/api/Book', bookRequest).subscribe({
        next: () => {
          console.log('Book added successfully');
          this.books$ = this.getBooks();
          this.resetForm();
        }
      });
    }
  }
  

  // Fetches books 
  private getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>('https://localhost:7275/api/Book');
  }

  // book deletion
  onDeleteBook(id: number): void {
    this.http.delete(`https://localhost:7275/api/Book/${id}`).subscribe({
      next: () => {
        console.log('Book deleted successfully');
        this.books$ = this.getBooks();
      }
    });
  }

  // Populates the form for updating a book
  onUpdateBook(book: Book): void {
    this.isUpdateMode = true; // Enable Update mode
    this.booksForm.patchValue(book); // Populate the form with book details
  }

  // Resets the form and toggles to Add mode
  private resetForm(): void {
    this.booksForm.reset();
    this.isUpdateMode = false; 
  }
}
