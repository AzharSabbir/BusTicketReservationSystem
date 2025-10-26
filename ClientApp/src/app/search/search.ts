import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { SearchService, AvailableBusDto } from '../services/search';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './search.html',
  styleUrls: ['./search.css']
})
export class SearchComponent {
  searchModel = {
    from: 'Dhaka',
    to: 'Rajshahi',
    journeyDate: this.getTodayDateString()
  };

  searchResults: AvailableBusDto[] = [];
  isLoading = false;
  searchError: string | null = null;
  hasSearched = false;

  totalBusesFound = 0;
  totalOperatorsFound = '--';
  totalSeatsAvailable = '--';

  currentSortCriteria = 'DEPARTURE TIME';
  sortAscending = true;

  constructor(
    private searchService: SearchService,
    private router: Router
  ) { }

  onSearchSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.searchError = null;
    this.searchResults = [];
    this.hasSearched = true;
    this.totalBusesFound = 0;

    this.searchService.searchAvailableBuses(
      this.searchModel.from,
      this.searchModel.to,
      this.searchModel.journeyDate
    ).subscribe({
      next: (results) => {
        this.searchResults = results;
        this.totalBusesFound = results.length;
        this.sortResults(this.currentSortCriteria, false);
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.searchError = 'An error occurred while searching. Please try again.';
        this.isLoading = false;
      }
    });
  }

  sortResults(criteria: string, toggleDirection = true): void {
    if (this.currentSortCriteria === criteria && toggleDirection) {
      this.sortAscending = !this.sortAscending;
    } else {
      this.currentSortCriteria = criteria;
      this.sortAscending = true;
    }

    this.searchResults.sort((a, b) => {
      let compare = 0;
      switch (criteria) {
        case 'DEPARTURE TIME':
          compare = new Date(a.startTime).getTime() - new Date(b.startTime).getTime();
          break;
        case 'AVAILABLE SEATS':
          compare = a.seatsLeft - b.seatsLeft;
          break;
        case 'FARE':
          compare = a.price - b.price;
          break;
      }
      return this.sortAscending ? compare : compare * -1;
    });
  }

  modifySearch(): void {
    this.searchResults = [];
    this.hasSearched = false;
    this.totalBusesFound = 0;
    console.log("Modify search clicked - clearing results for now.");
  }


  private getTodayDateString(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}