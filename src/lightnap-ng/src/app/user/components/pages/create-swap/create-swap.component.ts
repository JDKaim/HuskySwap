import { Component, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button'; // Import p-button module
import { DropdownModule } from 'primeng/dropdown'; // Import p-dropdown module
import { CardModule } from 'primeng/card';
import { FormsModule } from '@angular/forms'; // Import FormsModule for ngForm
import { DataService } from '../../../../trade-requests/services/data.service';
import { TradeRequest } from '../../../../trade-requests/models/response/trade-request';
import { CreateTradeRequestRequest } from '../../../../trade-requests/models/request/create-trade-request-request';
import { TradeRequestService } from 'src/app/trade-requests/services/trade-request.service';
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from 'primeng/tristatecheckbox';





@Component({
    selector: 'app-create-swap',
    standalone: true,
    templateUrl: './create-swap.component.html',
    imports: [ButtonModule, DropdownModule, CardModule, FormsModule],
    styleUrls: ['./create-swap.component.css']
})
export class CreateSwapComponent {
    private tradeRequestService = inject(TradeRequestService);
    // Property to store the selected class to offer
    offerClass: number | null = null;
  
    // Property to store the selected class to request
    requestClass: number | null = null;
  
    // Mock classes
    classOptions: { label: string, value: string }[] = [
      { label: 'Math 101', value: '1' },
      { label: 'CS 101', value: '2' },
      { label: 'History 101', value: '3' },
    ];

    // When user submits form
    onSubmit(): void {
        console.log('Offer Class:', this.offerClass);
        console.log('Request Class:', this.requestClass);
        const request: CreateTradeRequestRequest = {
            requestingClassUserId: this.requestClass,
            targetClassUserId: this.requestClass,
            status: "",
            notes: ""
        };
        this.tradeRequestService.createTradeRequest(request).subscribe(response => {
            console.log('Trade request created:', response);
        });
    }
}
