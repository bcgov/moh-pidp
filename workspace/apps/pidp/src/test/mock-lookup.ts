import { LookupConfig } from '@app/modules/lookup/lookup.types';

export class MockLookup {
  public static get(): LookupConfig {
    /* eslint-disable */
    // Export of /lookups response copied from PostMan:
    return {
      colleges: [
        {
          code: 1,
          name: 'College of Physicians and Surgeons of BC',
          acronym: 'CPSBC',
        },
        {
          code: 2,
          name: 'College of Pharmacists of BC',
          acronym: 'CPBC',
        },
        {
          code: 3,
          name: 'BC College of Nurses and Midwives',
          acronym: 'BCCNM',
        },
      ],
      countries: [
        {
          code: 'CA',
          name: 'Canada',
        },
        {
          code: 'US',
          name: 'United States',
        },
      ],
      provinces: [
        {
          code: 'WY',
          countryCode: 'US',
          name: 'Wyoming',
        },
        {
          code: 'CA',
          countryCode: 'US',
          name: 'California',
        },
        {
          code: 'CO',
          countryCode: 'US',
          name: 'Colorado',
        },
        {
          code: 'CT',
          countryCode: 'US',
          name: 'Connecticut',
        },
        {
          code: 'DE',
          countryCode: 'US',
          name: 'Delaware',
        },
        {
          code: 'DC',
          countryCode: 'US',
          name: 'District of Columbia',
        },
        {
          code: 'FL',
          countryCode: 'US',
          name: 'Florida',
        },
        {
          code: 'GA',
          countryCode: 'US',
          name: 'Georgia',
        },
        {
          code: 'GU',
          countryCode: 'US',
          name: 'Guam',
        },
        {
          code: 'HI',
          countryCode: 'US',
          name: 'Hawaii',
        },
        {
          code: 'ID',
          countryCode: 'US',
          name: 'Idaho',
        },
        {
          code: 'IN',
          countryCode: 'US',
          name: 'Indiana',
        },
        {
          code: 'IA',
          countryCode: 'US',
          name: 'Iowa',
        },
        {
          code: 'KS',
          countryCode: 'US',
          name: 'Kansas',
        },
        {
          code: 'AR',
          countryCode: 'US',
          name: 'Arkansas',
        },
        {
          code: 'KY',
          countryCode: 'US',
          name: 'Kentucky',
        },
        {
          code: 'AZ',
          countryCode: 'US',
          name: 'Arizona',
        },
        {
          code: 'AK',
          countryCode: 'US',
          name: 'Alaska',
        },
        {
          code: 'AB',
          countryCode: 'CA',
          name: 'Alberta',
        },
        {
          code: 'BC',
          countryCode: 'CA',
          name: 'British Columbia',
        },
        {
          code: 'MB',
          countryCode: 'CA',
          name: 'Manitoba',
        },
        {
          code: 'NB',
          countryCode: 'CA',
          name: 'New Brunswick',
        },
        {
          code: 'NL',
          countryCode: 'CA',
          name: 'Newfoundland and Labrador',
        },
        {
          code: 'NS',
          countryCode: 'CA',
          name: 'Nova Scotia',
        },
        {
          code: 'AS',
          countryCode: 'US',
          name: 'American Samoa',
        },
        {
          code: 'ON',
          countryCode: 'CA',
          name: 'Ontario',
        },
        {
          code: 'QC',
          countryCode: 'CA',
          name: 'Quebec',
        },
        {
          code: 'SK',
          countryCode: 'CA',
          name: 'Saskatchewan',
        },
        {
          code: 'NT',
          countryCode: 'CA',
          name: 'Northwest Territories',
        },
        {
          code: 'NU',
          countryCode: 'CA',
          name: 'Nunavut',
        },
        {
          code: 'YT',
          countryCode: 'CA',
          name: 'Yukon',
        },
        {
          code: 'AL',
          countryCode: 'US',
          name: 'Alabama',
        },
        {
          code: 'PE',
          countryCode: 'CA',
          name: 'Prince Edward Island',
        },
        {
          code: 'LA',
          countryCode: 'US',
          name: 'Louisiana',
        },
        {
          code: 'IL',
          countryCode: 'US',
          name: 'Illinois',
        },
        {
          code: 'MD',
          countryCode: 'US',
          name: 'Maryland',
        },
        {
          code: 'PR',
          countryCode: 'US',
          name: 'Puerto Rico',
        },
        {
          code: 'RI',
          countryCode: 'US',
          name: 'Rhode Island',
        },
        {
          code: 'SC',
          countryCode: 'US',
          name: 'South Carolina',
        },
        {
          code: 'ME',
          countryCode: 'US',
          name: 'Maine',
        },
        {
          code: 'TN',
          countryCode: 'US',
          name: 'Tennessee',
        },
        {
          code: 'TX',
          countryCode: 'US',
          name: 'Texas',
        },
        {
          code: 'PA',
          countryCode: 'US',
          name: 'Pennsylvania',
        },
        {
          code: 'UM',
          countryCode: 'US',
          name: 'United States Minor Outlying Islands',
        },
        {
          code: 'VT',
          countryCode: 'US',
          name: 'Vermont',
        },
        {
          code: 'VI',
          countryCode: 'US',
          name: 'Virgin Islands, U.S.',
        },
        {
          code: 'VA',
          countryCode: 'US',
          name: 'Virginia',
        },
        {
          code: 'WA',
          countryCode: 'US',
          name: 'Washington',
        },
        {
          code: 'WV',
          countryCode: 'US',
          name: 'West Virginia',
        },
        {
          code: 'WI',
          countryCode: 'US',
          name: 'Wisconsin',
        },
        {
          code: 'UT',
          countryCode: 'US',
          name: 'Utah',
        },
        {
          code: 'OR',
          countryCode: 'US',
          name: 'Oregon',
        },
        {
          code: 'SD',
          countryCode: 'US',
          name: 'South Dakota',
        },
        {
          code: 'OH',
          countryCode: 'US',
          name: 'Ohio',
        },
        {
          code: 'MA',
          countryCode: 'US',
          name: 'Massachusetts',
        },
        {
          code: 'OK',
          countryCode: 'US',
          name: 'Oklahoma',
        },
        {
          code: 'MN',
          countryCode: 'US',
          name: 'Minnesota',
        },
        {
          code: 'MS',
          countryCode: 'US',
          name: 'Mississippi',
        },
        {
          code: 'MO',
          countryCode: 'US',
          name: 'Missouri',
        },
        {
          code: 'MT',
          countryCode: 'US',
          name: 'Montana',
        },
        {
          code: 'NE',
          countryCode: 'US',
          name: 'Nebraska',
        },
        {
          code: 'MI',
          countryCode: 'US',
          name: 'Michigan',
        },
        {
          code: 'NH',
          countryCode: 'US',
          name: 'New Hampshire',
        },
        {
          code: 'NJ',
          countryCode: 'US',
          name: 'New Jersey',
        },
        {
          code: 'NM',
          countryCode: 'US',
          name: 'New Mexico',
        },
        {
          code: 'NY',
          countryCode: 'US',
          name: 'New York',
        },
        {
          code: 'NC',
          countryCode: 'US',
          name: 'North Carolina',
        },
        {
          code: 'ND',
          countryCode: 'US',
          name: 'North Dakota',
        },
        {
          code: 'MP',
          countryCode: 'US',
          name: 'Northern Mariana Islands',
        },
        {
          code: 'NV',
          countryCode: 'US',
          name: 'Nevada',
        },
      ],
    };
    /* eslint-enable */
  }
}
