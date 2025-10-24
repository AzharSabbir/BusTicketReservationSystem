import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { SearchService, AvailableBusDto } from '../services/search';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './search.html',
  styleUrls: ['./search.css']
})
export class SearchComponent {
  // 1. Variables to hold our form data
  searchModel = {
    from: 'Dhaka',
    to: 'Rajshahi',
    journeyDate: this.getTodayDateString() // Default to today
  };

  // 2. Variables to hold our results
  searchResults: AvailableBusDto[] = [];
  isLoading = false;
  searchError: string | null = null;

  // 3. Inject our new SearchService
  constructor(private searchService: SearchService) { }

  // 4. The function to call when the form is submitted
  onSearchSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.searchError = null;
    this.searchResults = []; // Clear old results

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

  // Helper function to get today's date in YYYY-MM-DD format
  private getTodayDateString(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}