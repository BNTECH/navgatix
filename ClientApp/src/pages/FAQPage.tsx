import { useState } from 'react';
import { Link } from 'react-router-dom';
import { ChevronDown, HelpCircle, MapPin, MessageCircle, ArrowRight } from 'lucide-react';

const faqData = [
    {
        category: "General",
        questions: [
            {
                q: "1️⃣ What services do you provide?",
                a: "We are a driver-powered logistics platform connecting customers, businesses, and logistics companies with verified drivers across India. We provide local parcel delivery, Mini truck & LCV booking, Heavy & big truck transport (14 ft to 32 ft), Full Truck Load (FTL) & Part Truck Load (PTL), and Enterprise logistics solutions."
            },
            {
                q: "5️⃣ Do you provide services across India?",
                a: "Yes. We offer local city transport as well as intercity and Pan-India truck booking services. Coverage includes: Metro cities, Tier 2 & Tier 3 cities, and Interstate logistics routes."
            },
            {
                q: "6️⃣ What types of vehicles are available?",
                a: "We provide a wide range of vehicles including: 2-Wheelers, 3-Wheelers/Cargo autos, Mini trucks (Tata Ace), Pickup trucks & LCV, 14 ft to 32 ft trucks, Single axle & multi-axle trucks, and Container trucks."
            },
        ]
    },
    {
        category: "Booking & Pricing",
        questions: [
            {
                q: "2️⃣ How do I book a vehicle?",
                a: "You can book directly through our website or mobile app by entering your pickup & drop location, selecting the vehicle type, confirming pricing, and scheduling pickup. Once booked, a nearby verified driver is assigned to your delivery."
            },
            {
                q: "7️⃣ What is the difference between FTL and PTL?",
                a: "FTL (Full Truck Load): You book the entire truck exclusively for your shipment. PTL (Part Truck Load): You share truck space and pay only for the portion you use. FTL is ideal for bulk goods. PTL is cost-effective for smaller shipments."
            },
            {
                q: "8️⃣ How are delivery charges calculated?",
                a: "Pricing depends on distance, vehicle type, load weight/size, route & toll charges, and urgency (standard or express). We provide transparent pricing before booking confirmation."
            },
            {
                q: "9️⃣ Is my shipment insured?",
                a: "Insurance availability depends on the selected service. For heavy and long-distance transport, insurance options can be provided upon request."
            }
        ]
    },
    {
        category: "Tracking & Drivers",
        questions: [
            {
                q: "3️⃣ How can I track my vehicle and shipment?",
                a: "We provide real-time live tracking for all active deliveries integrated with Google Maps/Location APIs. You can track live vehicle location, monitor route, see ETA, and get trip updates directly from your dashboard or app."
            },
            {
                q: "4️⃣ Are your drivers verified?",
                a: "Yes. All drivers undergo identity verification, document verification (DL, RC, permits), background checks, and vehicle document validation. We ensure only verified and compliant drivers operate on our platform."
            },
            {
                q: "🔟 How can drivers join the platform?",
                a: "Drivers and vehicle owners can register through our Driver Partner section by submitting their Driving License, Vehicle RC, permits, and Bank details. Once verified, they can start accepting delivery trips and earning."
            }
        ]
    }
];

const FAQPage = () => {
    const [openIndex, setOpenIndex] = useState<{ cat: number, q: number } | null>(null);

    const toggleAccordion = (catIdx: number, qIdx: number) => {
        if (openIndex?.cat === catIdx && openIndex?.q === qIdx) {
            setOpenIndex(null); // Close if clicking the same one
        } else {
            setOpenIndex({ cat: catIdx, q: qIdx });
        }
    };

    return (
        <section className="py-24 bg-white relative font-sans">
            <div className="absolute top-0 right-0 w-1/3 h-1/3 bg-primary-50 rounded-bl-full opacity-50"></div>

            <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
                <div className="text-center mb-16">
                    <div className="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-primary-100 text-primary-600 mb-6">
                        <HelpCircle className="h-8 w-8" />
                    </div>
                    <h2 className="text-3xl md:text-5xl font-extrabold text-slate-900 mb-6">
                        Frequently Asked Questions
                    </h2>
                    <p className="text-xl text-slate-500">
                        Everything you need to know about <span className="font-bold text-primary-600">Navgatix</span> services and operations.
                    </p>
                </div>

                <div className="space-y-12">
                    {faqData.map((category, catIdx) => (
                        <div key={catIdx}>
                            <h3 className="text-2xl font-bold text-slate-800 mb-6 border-b border-slate-100 pb-2">
                                {category.category}
                            </h3>
                            <div className="space-y-4">
                                {category.questions.map((item, qIdx) => {
                                    const isOpen = openIndex?.cat === catIdx && openIndex?.q === qIdx;
                                    return (
                                        <div
                                            key={qIdx}
                                            className={`premium-card overflow-hidden transition-all duration-300 ${isOpen ? 'ring-2 ring-primary-500/20 border-primary-200' : ''}`}
                                        >
                                            <button
                                                className="w-full px-6 py-5 text-left flex justify-between items-center bg-white hover:bg-slate-50 transition-colors focus:outline-none"
                                                onClick={() => toggleAccordion(catIdx, qIdx)}
                                            >
                                                <span className={`font-semibold text-lg pr-8 ${isOpen ? 'text-primary-700' : 'text-slate-700'}`}>
                                                    {item.q}
                                                </span>
                                                <ChevronDown className={`h-5 w-5 shrink-0 transition-transform duration-300 ${isOpen ? 'rotate-180 text-primary-600' : 'text-slate-400'}`} />
                                            </button>
                                            <div
                                                className={`px-6 pb-6 text-slate-600 leading-relaxed bg-white border-t border-slate-50 transition-all duration-300 overflow-hidden ${isOpen ? 'block opacity-100 mt-2' : 'hidden opacity-0 h-0 p-0 m-0'}`}
                                            >
                                                {item.a}
                                            </div>
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    ))}
                </div>

                <div className="mt-16 rounded-3xl border border-primary-100 bg-gradient-to-r from-primary-50 to-white p-8 md:p-10 shadow-lg shadow-primary-100/50">
                    <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-6">
                        <div className="max-w-2xl">
                            <div className="inline-flex items-center gap-2 rounded-full bg-white px-4 py-2 text-sm font-semibold text-primary-700 border border-primary-100 mb-4">
                                <MessageCircle className="h-4 w-4" />
                                Need More Help?
                            </div>
                            <h3 className="text-2xl md:text-3xl font-extrabold text-slate-900 mb-3">
                                Have more queries? Ask us directly.
                            </h3>
                            <p className="text-slate-600 text-lg">
                                If your question is not listed above, go to the contact section and send us your message. Our team will get back to you.
                            </p>
                        </div>
                        <Link
                            to="/contact"
                            className="inline-flex items-center justify-center gap-2 rounded-2xl bg-primary-600 px-6 py-4 text-white font-bold shadow-lg shadow-primary-500/25 hover:bg-primary-500 transition-colors"
                        >
                            Ask Your Question
                            <ArrowRight className="h-5 w-5" />
                        </Link>
                    </div>
                </div>

                {/* Operating Areas Banner */}
                <div className="mt-20 bg-gradient-to-r from-slate-900 to-slate-800 rounded-3xl p-8 md:p-12 text-center text-white shadow-xl relative overflow-hidden">
                    <div className="absolute top-0 right-0 opacity-10 translate-x-1/4 -translate-y-1/4">
                        <MapPin className="h-64 w-64 text-white" />
                    </div>
                    <div className="relative z-10 flex flex-col md:flex-row items-center justify-between gap-8">
                        <div className="text-left max-w-lg">
                            <h4 className="text-2xl font-bold mb-3 flex items-center gap-2">
                                <MapPin className="h-6 w-6 text-primary-400" /> Currently Operating In
                            </h4>
                            <p className="text-slate-300 mb-6 text-lg">We are actively serving key locations and rapidly expanding across North India.</p>
                            <div className="flex flex-wrap gap-3">
                                {['Sirsa', 'Chandigarh', 'Ludhiana', 'Mohali'].map((city, idx) => (
                                    <span key={idx} className="bg-white/10 backdrop-blur border border-white/20 text-white px-4 py-2 rounded-lg font-medium">
                                        {city}
                                    </span>
                                ))}
                            </div>
                        </div>
                        <div className="w-full md:w-auto shrink-0 rounded-[1.75rem] border border-white/15 bg-white/8 px-7 py-6 text-center shadow-inner shadow-white/5 backdrop-blur-sm">
                            <div className="inline-flex items-center gap-2 rounded-full border border-primary-400/30 bg-primary-400/10 px-3 py-1 text-xs font-semibold uppercase tracking-[0.2em] text-primary-200">
                                Coverage Update
                            </div>
                            <h5 className="mt-4 font-bold text-2xl text-white">Expanding Soon</h5>
                            <p className="mt-2 max-w-[15rem] text-sm leading-6 text-slate-300">More cities across North India are being added in the next rollout.</p>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    );
};

export default FAQPage;
