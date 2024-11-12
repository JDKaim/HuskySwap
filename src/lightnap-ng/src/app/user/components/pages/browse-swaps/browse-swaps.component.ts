import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';
interface ClassOffer {
    offeredClass: string;
    wantedClass: string;
    offerer: string;
  }
@Component({
    selector: 'app-browse-swaps',
    standalone: true,
    templateUrl: './browse-swaps.component.html',
    imports: [CardModule, CommonModule]
})
export class BrowseSwapsComponent {
    offers: ClassOffer[] = [
        { offeredClass: 'CSE 442', wantedClass: 'CSE 403' , offerer: "Chicken Bake"},
        { offeredClass: 'CSE 331', wantedClass: 'CSE 333' , offerer: "Rizzler"},
        { offeredClass: 'CSE 123', wantedClass: 'CSE 421' , offerer: "Costco Guy"}
      ];
   
      acceptOffer(offer: ClassOffer) {
        console.log(`Accepted offer: ${offer.offeredClass} for ${offer.wantedClass} from ${offer.offerer}`);
      }
   
      declineOffer(offer: ClassOffer) {
        console.log(`Declined offer: ${offer.offeredClass} for ${offer.wantedClass} from ${offer.offerer}`);
      }
}
