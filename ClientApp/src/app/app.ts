import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { SearchService, AvailableBusDto } from './services/search';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {

  searchModel = {
    from: 'Dhaka',
    to: 'Rajshahi',
    journeyDate: this.getTodayDateString()
  };

  searchResults: AvailableBusDto[] = [];
  isLoading = false;
  searchError: string | null = null;

  constructor(private searchService: SearchService) { }

  onSearchSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.searchError = null;
    this.searchResults = [];

    this.searchService.searchAvailableBuses(
      this.searchModel.from,
      this.searchModel.to,
      this.searchModel.journeyDate
    ).subscribe({
      next: (results) => {
        this.searchResults = results;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.searchError = 'An error occurred while searching. Please try again.';
        this.isLoading = false;
      }
    });
  }

  private getTodayDateString(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}