import { Box, UserCheck, ShieldCheck, Map, Clock, BadgeCheck, CheckCircle2 } from 'lucide-react';

const ServicesPage = () => {
    return (
        <section id="solutions" className="py-24 bg-slate-50 relative">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">

                <div className="text-center mb-20">
                    <h2 className="text-sm font-bold text-primary-600 tracking-wider uppercase mb-3 flex items-center justify-center gap-3">
                        <img src="/logo.png" alt="Navgatix" className="h-10 object-contain drop-shadow-sm" /> Our Services
                    </h2>
                    <h3 className="text-3xl md:text-5xl font-extrabold text-slate-900 mb-6">
                        India’s Smart Driver & Logistics Network
                    </h3>
                    <p className="text-xl text-slate-600 max-w-3xl mx-auto leading-relaxed">
                        We connect businesses and individuals who need reliable delivery with verified drivers who want to earn.
                        Our platform enables fast, affordable, and scalable logistics solutions across India.
                    </p>
                </div>

                {/* Service Block 1 */}
                <div className="grid md:grid-cols-2 gap-12 items-center mb-24">
                    <div className="order-2 md:order-1">
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-primary-50 text-primary-700 font-semibold mb-6">
                            <Box className="h-5 w-5" /> For Businesses & Individuals
                        </div>
                        <h4 className="text-3xl font-bold text-slate-900 mb-6">Deliver Anything, Anytime</h4>
                        <p className="text-lg text-slate-600 mb-8 leading-relaxed">
                            Whether you are a company, shop owner, warehouse, e-commerce seller, or an individual — we make delivery simple.
                        </p>

                        <div className="grid grid-cols-2 gap-x-6 gap-y-4 mb-8">
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Parcels & documents</span></div>
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Retail & e-commerce</span></div>
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Kirana & FMCG supplies</span></div>
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Furniture shifting</span></div>
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Industrial goods</span></div>
                            <div className="flex items-start gap-3"><CheckCircle2 className="h-5 w-5 text-primary-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Full truckload (FTL/PTL)</span></div>
                        </div>

                        <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm">
                            <h5 className="font-bold text-slate-900 mb-3">Vehicle Options Available:</h5>
                            <p className="text-slate-600 text-sm leading-relaxed">
                                2-Wheelers, Cargo Autos, Mini Trucks (Tata Ace), Pickups, LCV, 14ft to 32ft Trucks, Container & Multi-Axle Trucks.
                            </p>
                        </div>
                    </div>
                    <div className="order-1 md:order-2 bg-gradient-to-br from-primary-100 to-green-50 rounded-[2.5rem] p-8 md:p-12 relative overflow-hidden h-full min-h-[400px] flex items-center justify-center">
                        <div className="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?q=80&w=2070&auto=format&fit=crop')] bg-cover bg-center mix-blend-overlay opacity-30"></div>
                        <div className="relative z-10 grid grid-cols-2 gap-4 w-full">
                            {['On-demand booking', 'Real-time GPS tracking', 'Transparent pricing', 'Pan-India coverage'].map((feature, idx) => (
                                <div key={idx} className="bg-white/90 backdrop-blur p-4 rounded-xl shadow-lg border border-white/50 flex flex-col items-center text-center gap-2 hover:-translate-y-1 transition-transform">
                                    <BadgeCheck className="h-8 w-8 text-primary-600" />
                                    <span className="font-bold text-slate-800 text-sm">{feature}</span>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>

                {/* Service Block 2 */}
                <div className="grid md:grid-cols-2 gap-12 items-center mb-24">
                    <div className="bg-slate-900 rounded-[2.5rem] p-8 md:p-12 relative overflow-hidden h-full min-h-[400px] flex flex-col justify-center">
                        <div className="absolute right-0 bottom-0 opacity-10 translate-x-1/4 translate-y-1/4">
                            <ShieldCheck className="h-64 w-64 text-white" />
                        </div>
                        <div className="relative z-10 text-white">
                            <h3 className="text-3xl font-bold mb-6">Platform Benefits</h3>
                            <ul className="space-y-4">
                                {['Access to verified drivers', 'On-demand vehicle availability', 'Dedicated fleet support', 'Route-based deployment', 'API integration options', 'Centralized tracking dashboard'].map((benefit, idx) => (
                                    <li key={idx} className="flex items-center gap-3">
                                        <div className="w-8 h-8 rounded-lg bg-primary-600/20 flex items-center justify-center">
                                            <CheckCircle2 className="h-5 w-5 text-primary-400" />
                                        </div>
                                        <span className="text-slate-300 font-medium">{benefit}</span>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>
                    <div>
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-indigo-50 text-indigo-700 font-semibold mb-6">
                            <Map className="h-5 w-5" /> For Logistics Companies
                        </div>
                        <h4 className="text-3xl font-bold text-slate-900 mb-6">Expand Your Delivery Capacity</h4>
                        <p className="text-lg text-slate-600 mb-8 leading-relaxed">
                            Scale your operations without investing in your own fleet. Grow your logistics network without increasing fixed costs.
                        </p>

                        <div className="mb-8">
                            <h5 className="font-bold text-slate-900 mb-4">We actively support:</h5>
                            <div className="flex flex-wrap gap-3">
                                {['3PL companies', 'Transport contractors', 'E-commerce logistics', 'Warehouse operators', 'Manufacturers & distributors'].map((tag, idx) => (
                                    <span key={idx} className="bg-white border border-slate-200 px-4 py-2 rounded-lg text-slate-700 font-medium text-sm shadow-sm">
                                        {tag}
                                    </span>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>

                {/* Service Block 3 */}
                <div className="grid md:grid-cols-2 gap-12 items-center mb-24">
                    <div className="order-2 md:order-1">
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-emerald-50 text-emerald-700 font-semibold mb-6">
                            <UserCheck className="h-5 w-5" /> For Drivers & Truck Owners
                        </div>
                        <h4 className="text-3xl font-bold text-slate-900 mb-6">Earn with Flexible Opportunities</h4>
                        <p className="text-lg text-slate-600 mb-8 leading-relaxed">
                            Join our growing driver network and earn consistently. We provide delivery trips across India for all vehicle types.
                        </p>

                        <div className="grid grid-cols-2 gap-x-6 gap-y-4 mb-8">
                            <div className="flex items-start gap-3"><Clock className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Flexible hours</span></div>
                            <div className="flex items-start gap-3"><BadgeCheck className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Regular orders</span></div>
                            <div className="flex items-start gap-3"><BadgeCheck className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Transparent earnings</span></div>
                            <div className="flex items-start gap-3"><BadgeCheck className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Weekly payouts</span></div>
                            <div className="flex items-start gap-3"><BadgeCheck className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">Incentives & bonuses</span></div>
                            <div className="flex items-start gap-3"><ShieldCheck className="h-5 w-5 text-emerald-500 mt-1 shrink-0" /><span className="text-slate-700 font-medium">24/7 support</span></div>
                        </div>

                        <div className="bg-emerald-600 text-white p-6 rounded-2xl shadow-lg mt-8 text-center">
                            <h3 className="text-2xl font-black tracking-tight">Drive. Deliver. Earn.</h3>
                        </div>
                    </div>
                    <div className="order-1 md:order-2 bg-slate-200 rounded-[2.5rem] relative overflow-hidden h-full min-h-[400px]">
                        <img src="https://images.unsplash.com/photo-1601584115197-04ecc0da31d7?q=80&w=2670&auto=format&fit=crop" alt="Driver" className="absolute inset-0 w-full h-full object-cover" />
                        <div className="absolute inset-0 bg-gradient-to-t from-slate-900/80 via-transparent to-transparent"></div>
                        <div className="absolute bottom-0 left-0 p-8 w-full">
                            <div className="glass-effect rounded-xl p-6 text-white border-white/20">
                                <h4 className="font-bold text-lg mb-2 text-white">Drive with any vehicle</h4>
                                <p className="text-sm text-slate-200">Bikes, Autos, Mini Trucks to Heavy Trucks.</p>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Professional Fleet Section */}
                <div className="mb-24">
                    <div className="text-center mb-16">
                        <h3 className="text-3xl font-bold text-slate-900 mb-4">India’s Most Versatile Fleet</h3>
                        <p className="text-slate-600 max-w-2xl mx-auto">From last-mile bike deliveries to pan-India multi-axle freight, we have the right vehicle for every load.</p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                        {[
                            { name: '2-Wheelers', desc: 'Ideal for small parcels, documents & food delivery.', icon: '🏍️' },
                            { name: '3-Wheelers / Cargo Autos', desc: 'Perfect for retail supply & city-wide logistics.', icon: '🛺' },
                            { name: 'Mini Trucks (Tata Ace)', desc: 'The backbone of e-commerce & FMCG distribution.', icon: '🚚' },
                            { name: 'Pickup & LCV Vehicles', desc: 'Versatile payload capacity for home shifting & industrial goods.', icon: '🚛' },
                            { name: '14 ft to 32 ft Trucks', desc: 'Optimized for mid-range and bulk transport requirements.', icon: '🚛' },
                            { name: 'Container & Multi-Axle', desc: 'Heavy-duty transport for industrial & full truckload freight.', icon: '🚢' }
                        ].map((v, i) => (
                            <div key={i} className="bg-white p-8 rounded-3xl border border-slate-200 shadow-sm hover:shadow-xl hover:border-primary-200 transition-all group">
                                <div className="text-5xl mb-6 group-hover:scale-110 transition-transform">{v.icon}</div>
                                <h4 className="text-xl font-bold text-slate-900 mb-3">{v.name}</h4>
                                <p className="text-slate-500 text-sm leading-relaxed">{v.desc}</p>
                            </div>
                        ))}
                    </div>
                </div>

                {/* About Us Summary */}
                <div className="mb-24 bg-slate-900 rounded-[2.5rem] p-10 md:p-16 text-center text-white relative overflow-hidden shadow-2xl">
                    <div className="absolute inset-0 bg-[radial-gradient(circle_at_50%_0%,rgba(34,197,94,0.15),transparent)]" />
                    <div className="relative z-10 max-w-4xl mx-auto">
                        <img src="/logo.png" alt="Navgatix Logo" className="h-24 mx-auto mb-6 drop-shadow-lg" />
                        <h3 className="text-3xl md:text-5xl font-black mb-6 tracking-tight">Driving India's Future</h3>
                        <p className="text-xl text-slate-300 font-medium mb-8 leading-relaxed">
                            At Navgatix, we eliminate friction in supply chains. We empower drivers with flexible earning opportunities and provide businesses with reliable, scalable logistics networks.
                        </p>
                        <a href="/about" className="inline-flex items-center justify-center bg-primary-600 hover:bg-primary-500 text-white font-bold px-8 py-4 rounded-xl transition-all shadow-lg shadow-primary-500/30 hover:-translate-y-1">
                            Discover Our Full Story
                        </a>
                    </div>
                </div>

                {/* Contact Us Section */}
                <div id="contact" className="max-w-6xl mx-auto bg-white rounded-[2.5rem] p-8 md:p-16 border border-slate-200 shadow-xl flex flex-col md:flex-row gap-12 items-center relative overflow-hidden">
                    <div className="absolute -top-24 -right-24 bg-primary-50 w-64 h-64 rounded-full blur-3xl opacity-50"></div>
                    <div className="flex-1 text-center md:text-left relative z-10">
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-blue-50 text-blue-700 font-bold mb-6 text-sm">
                            24/7 Support
                        </div>
                        <h3 className="text-3xl md:text-4xl font-extrabold text-slate-900 mb-4 tracking-tight">Need Help or Have Questions?</h3>
                        <p className="text-lg text-slate-600 font-medium mb-8 leading-relaxed">
                            Our dedicated support team is available around the clock to assist our drivers and logistics partners with any issues.
                        </p>
                        <div className="space-y-6">
                            <div className="flex items-center justify-center md:justify-start gap-4">
                                <div className="w-12 h-12 rounded-xl bg-primary-50 flex items-center justify-center text-primary-600 text-xl shadow-inner border border-primary-100">📞</div>
                                <div className="text-left">
                                    <p className="text-xs font-bold text-slate-400 tracking-wider">CALL US</p>
                                    <p className="text-slate-900 font-bold text-lg">+91 98765 43210</p>
                                </div>
                            </div>
                            <div className="flex items-center justify-center md:justify-start gap-4">
                                <div className="w-12 h-12 rounded-xl bg-blue-50 flex items-center justify-center text-blue-600 text-xl shadow-inner border border-blue-100">✉️</div>
                                <div className="text-left">
                                    <p className="text-xs font-bold text-slate-400 tracking-wider">EMAIL US</p>
                                    <p className="text-slate-900 font-bold text-lg">support@navgatix.com</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="flex-1 w-full relative z-10">
                        <div className="absolute inset-0 bg-gradient-to-br from-primary-500 to-blue-600 rounded-3xl rotate-2 opacity-15 transform scale-105"></div>
                        <form className="relative bg-white p-8 border border-slate-100 shadow-lg rounded-3xl space-y-5">
                            <div>
                                <label className="block text-xs font-bold text-slate-500 mb-2 ml-1">FULL NAME</label>
                                <input type="text" placeholder="John Doe" className="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3.5 focus:outline-none focus:border-primary-500 focus:ring-2 focus:ring-primary-500/20 font-medium transition-all" />
                            </div>
                            <div>
                                <label className="block text-xs font-bold text-slate-500 mb-2 ml-1">EMAIL ADDRESS</label>
                                <input type="email" placeholder="john@example.com" className="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3.5 focus:outline-none focus:border-primary-500 focus:ring-2 focus:ring-primary-500/20 font-medium transition-all" />
                            </div>
                            <div>
                                <label className="block text-xs font-bold text-slate-500 mb-2 ml-1">MESSAGE</label>
                                <textarea placeholder="How can we help?" rows={4} className="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3.5 focus:outline-none focus:border-primary-500 focus:ring-2 focus:ring-primary-500/20 font-medium resize-none transition-all"></textarea>
                            </div>
                            <button type="button" className="w-full bg-slate-900 hover:bg-primary-600 text-white font-bold py-4 rounded-xl transition-all shadow-md hover:shadow-lg shadow-slate-900/20">Send Message</button>
                        </form>
                    </div>
                </div>

            </div>
        </section>
    );
};

export default ServicesPage;
