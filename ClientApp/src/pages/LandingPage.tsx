import { Link } from 'react-router-dom';
import { ArrowRight, Truck, Shield, Clock, MapPin, Search, Cpu, Navigation, Eye, CheckCircle2 } from 'lucide-react';
import ContactUs from '../components/ContactUs';

const LandingPage = () => {
    return (
        <div className="min-h-screen bg-slate-50 font-sans selection:bg-primary-500/30">
            {/* Hero Section */}
            <header className="relative overflow-hidden bg-slate-900 border-b border-slate-800">
                {/* Background Decorative Elements */}
                <div className="absolute top-0 right-0 -mr-20 -mt-20 w-96 h-96 rounded-full bg-primary-100/50 blur-3xl"></div>
                <div className="absolute bottom-0 left-0 -ml-20 -mb-20 w-80 h-80 rounded-full bg-slate-100/50 blur-3xl"></div>

                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
                    <div className="text-center max-w-4xl mx-auto py-20">
                        <h1 className="text-5xl md:text-7xl font-extrabold tracking-tight text-white mb-8 leading-tight">
                            The Intelligent <br />
                            <span className="text-transparent bg-clip-text bg-gradient-to-r from-primary-400 to-indigo-400">Logistics Platform</span>
                        </h1>
                        <p className="text-xl md:text-2xl text-slate-300 mb-10 leading-relaxed max-w-3xl mx-auto font-medium">
                            Streamline your fleet operations, track shipments in real-time, and make data-driven decisions with our premium logistics management solution.
                        </p>

                        <div className="flex flex-col md:flex-row justify-center items-stretch gap-6 mt-12 max-w-4xl mx-auto w-full">
                            {/* Driver Card */}
                            <div className="flex-1 min-w-0 rounded-3xl p-8 text-left relative overflow-hidden group hover:shadow-2xl transition-all duration-300 border-l-4 border-primary-500" style={{ background: 'linear-gradient(135deg, #0d1f14 0%, #1a3020 50%, #0f2318 100%)', boxShadow: '0 4px 32px 0 rgba(34,197,94,0.10), inset 0 0 0 1px rgba(34,197,94,0.15)' }}>
                                <div className="absolute right-0 bottom-0 opacity-10 translate-x-1/4 translate-y-1/4 group-hover:scale-110 transition-transform duration-500">
                                    <Truck className="h-48 w-48 text-primary-400" />
                                </div>
                                <div className="relative z-10">
                                    <div className="w-14 h-14 bg-primary-600 rounded-2xl flex items-center justify-center mb-8 shadow-lg shadow-primary-900/50 ring-2 ring-primary-500/30">
                                        <Truck className="h-7 w-7 text-white" />
                                    </div>
                                    <h3 className="text-3xl md:text-4xl font-extrabold text-white mb-4">
                                        I am a <span className="text-primary-400 underline decoration-primary-400/40">Driver</span>
                                    </h3>
                                    <p className="text-slate-300 text-[15px] mb-10 leading-relaxed">
                                        Join our network. Get regular loads, set your own routes, and earn with transparency. Transporters can manage their entire fleet.
                                    </p>
                                    <Link to="/register?type=driver" className="inline-flex items-center gap-2 bg-primary-600 hover:bg-primary-500 text-white font-bold text-sm px-5 py-2.5 rounded-xl transition-colors shadow-md shadow-primary-900/40">
                                        Register as Partner
                                        <ArrowRight className="h-4 w-4" />
                                    </Link>
                                </div>
                            </div>

                            {/* Deliver Card */}
                            <div className="flex-1 min-w-0 bg-white rounded-3xl p-8 text-left border border-slate-200 shadow-xl shadow-slate-200/50 group hover:-translate-y-1 transition-all duration-300 relative overflow-hidden">
                                <div className="absolute right-0 bottom-0 opacity-[0.03] translate-x-1/8 translate-y-1/8 group-hover:scale-110 transition-transform duration-500">
                                    <svg viewBox="0 0 24 24" fill="currentColor" className="h-64 w-64 text-slate-900">
                                        <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5" />
                                    </svg>
                                </div>
                                <div className="relative z-10">
                                    <div className="w-14 h-14 bg-emerald-50 rounded-2xl flex items-center justify-center mb-8">
                                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" className="h-7 w-7 text-emerald-600">
                                            <path strokeLinecap="round" strokeLinejoin="round" d="M17 20h5V10l-5-5H7L2 10v10h5m5-10v10m5-10V5m-5 5v5" />
                                        </svg>
                                    </div>
                                    <h3 className="text-3xl md:text-4xl font-extrabold text-slate-900 mb-4">
                                        I am a <span className="text-emerald-600 underline decoration-emerald-600/30">Customer (Logistics)</span>
                                    </h3>
                                    <p className="text-slate-500 text-[15px] mb-10 leading-relaxed">
                                        Book vehicles on-demand for home shifting, business supply, or industrial freight. Real-time tracking included.
                                    </p>
                                    <Link to="/register?type=customer" className="inline-flex items-center gap-2 text-emerald-600 font-bold text-sm hover:text-emerald-700 transition-colors">
                                        Book a Vehicle
                                        <ArrowRight className="h-4 w-4" />
                                    </Link>
                                </div>
                            </div>
                        </div>

                        {/* Quick Search Bar (Visual only for landing) */}
                        <div className="mt-16 max-w-2xl mx-auto">
                            <div className="glass-effect p-2 rounded-2xl shadow-xl flex items-center">
                                <Search className="h-5 w-5 text-slate-400 ml-3" />
                                <input
                                    type="text"
                                    placeholder="Track a shipment (e.g., AWB number)"
                                    className="w-full bg-transparent border-none focus:outline-none px-4 py-3 text-slate-700 placeholder-slate-400"
                                />
                                <button className="bg-slate-900 hover:bg-slate-800 text-white px-6 py-3 rounded-xl font-medium transition-colors">
                                    Track
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </header>

            {/* Intelligent Features Section */}
            <section className="py-24 bg-[#0f172a] relative overflow-hidden">
                <div className="absolute top-0 left-0 w-full h-px bg-gradient-to-r from-transparent via-primary-500/30 to-transparent"></div>
                <div className="absolute top-0 left-0 w-full h-full bg-[radial-gradient(circle_at_30%_50%,rgba(34,197,94,0.05),transparent)]" />
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
                    <div className="text-center mb-16">
                        <h2 className="text-primary-500 font-bold uppercase tracking-widest text-sm mb-4">Modern Logistics</h2>
                        <h3 className="text-3xl md:text-5xl font-extrabold text-white">Engineered for Efficiency</h3>
                    </div>

                    <div className="grid md:grid-cols-3 gap-8">
                        <div className="bg-slate-900/40 backdrop-blur-sm border border-white/5 p-10 rounded-[2.5rem] hover:bg-slate-900/60 transition-all group">
                            <Cpu className="h-10 w-10 text-primary-500 mb-8 group-hover:scale-110 transition-transform" />
                            <h4 className="text-2xl font-bold text-white mb-4">Intelligent Driver Matching</h4>
                            <p className="text-slate-400 leading-relaxed font-medium">
                                Our sophisticated algorithm identifies and assigns the nearest available driver in real-time to reduce pickup latency and optimize delivery costs.
                            </p>
                        </div>
                        <div className="bg-slate-900/40 backdrop-blur-sm border border-white/5 p-10 rounded-[2.5rem] hover:bg-slate-900/60 transition-all group">
                            <Navigation className="h-10 w-10 text-primary-500 mb-8 group-hover:scale-110 transition-transform" />
                            <h4 className="text-2xl font-bold text-white mb-4">Smart Route Optimization</h4>
                            <p className="text-slate-400 leading-relaxed font-medium">
                                Dynamic pathfinding calculated via advanced mapping APIs ensures your cargo reaches its destination through the most fuel-efficient and speed-optimized routes.
                            </p>
                        </div>
                        <div className="bg-slate-900/40 backdrop-blur-sm border border-white/5 p-10 rounded-[2.5rem] hover:bg-slate-900/60 transition-all group">
                            <Eye className="h-10 w-10 text-primary-500 mb-8 group-hover:scale-110 transition-transform" />
                            <h4 className="text-2xl font-bold text-white mb-4">Real-Time Monitoring</h4>
                            <p className="text-slate-400 leading-relaxed font-medium">
                                Experience total transparency with live map integration and millisecond-accurate location tracking throughout the entire shipment lifecycle.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* Businesses We Support */}
            <section id="solutions" className="py-24 bg-white">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="grid lg:grid-cols-2 gap-16 items-center">
                        <div>
                            <h2 className="text-primary-600 font-bold uppercase tracking-widest text-sm mb-4">Our Partners</h2>
                            <h3 className="text-4xl font-extrabold text-slate-900 mb-6 leading-tight">Empowering growth across all business scales</h3>
                            <p className="text-lg text-slate-600 mb-10 leading-relaxed">
                                From local shops to global manufacturing giants, Navgatix provides the infrastructure to move anything, anywhere in India.
                            </p>

                            <div className="grid sm:grid-cols-2 gap-4">
                                {[
                                    { name: 'Retail Stores', desc: 'Last-mile replenishment' },
                                    { name: 'E-commerce Sellers', desc: 'Hyper-local delivery units' },
                                    { name: 'Warehouses', desc: 'Regional distribution' },
                                    { name: 'Distributors', desc: 'Bulk stock movement' },
                                    { name: 'Manufacturers', desc: 'Heavy industrial freight' }
                                ].map((b, i) => (
                                    <div key={i} className="flex items-center gap-4 bg-slate-50 p-4 rounded-2xl border border-slate-100">
                                        <div className="w-10 h-10 bg-white rounded-xl shadow-sm flex items-center justify-center text-primary-600">
                                            <CheckCircle2 className="h-5 w-5" />
                                        </div>
                                        <div>
                                            <p className="font-bold text-slate-900">{b.name}</p>
                                            <p className="text-xs text-slate-500">{b.desc}</p>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                        <div className="relative">
                            <div className="absolute -inset-4 bg-primary-100/50 rounded-[3rem] blur-2xl opacity-50" />
                            <div className="relative bg-slate-100 rounded-[3rem] aspect-square overflow-hidden border-8 border-white shadow-2xl">
                                <img
                                    src="https://images.unsplash.com/photo-1580674285054-bed31e145f59?q=80&w=2070&auto=format&fit=crop"
                                    alt="Business Logistics"
                                    className="w-full h-full object-cover"
                                />
                                <div className="absolute inset-0 bg-gradient-to-t from-slate-900/40 to-transparent" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* Features Grid */}
            <section id="features" className="py-24 bg-slate-50 relative">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="text-center mb-16">
                        <h2 className="text-sm font-bold text-primary-600 tracking-wider uppercase mb-3">Core Infrastructure</h2>
                        <h3 className="text-3xl md:text-4xl font-bold text-slate-900">Everything you need to manage your fleet</h3>
                    </div>

                    <div className="grid md:grid-cols-3 gap-8">
                        {/* Feature 1 */}
                        <div className="premium-card p-8 hover:-translate-y-2 transition-transform duration-300">
                            <div className="w-14 h-14 bg-blue-50 rounded-2xl flex items-center justify-center mb-6 text-blue-600">
                                <MapPin className="h-7 w-7" />
                            </div>
                            <h4 className="text-xl font-bold text-slate-900 mb-3">Pan-India Network</h4>
                            <p className="text-slate-600 leading-relaxed">
                                Access a massive network of verified drivers across every major city and rural hub in the country.
                            </p>
                        </div>

                        {/* Feature 2 */}
                        <div className="premium-card p-8 hover:-translate-y-2 transition-transform duration-300">
                            <div className="w-14 h-14 bg-emerald-50 rounded-2xl flex items-center justify-center mb-6 text-emerald-600">
                                <Clock className="h-7 w-7" />
                            </div>
                            <h4 className="text-xl font-bold text-slate-900 mb-3">On-Demand Scaling</h4>
                            <p className="text-slate-600 leading-relaxed">
                                Instantly scale your delivery capacity up or down based on seasonal demand without fixed overheads.
                            </p>
                        </div>

                        {/* Feature 3 */}
                        <div className="premium-card p-8 hover:-translate-y-2 transition-transform duration-300">
                            <div className="w-14 h-14 bg-purple-50 rounded-2xl flex items-center justify-center mb-6 text-purple-600">
                                <Shield className="h-7 w-7" />
                            </div>
                            <h4 className="text-xl font-bold text-slate-900 mb-3">Transparent Pricing</h4>
                            <p className="text-slate-600 leading-relaxed">
                                Professional quote matching system ensures you always get competitive, market-driven shipping rates.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* CTA Section */}
            <section className="py-24 relative overflow-hidden bg-slate-900">
                <div className="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1519003722811-923719003dbd?q=80&w=2670&auto=format&fit=crop')] bg-cover bg-center opacity-10 mix-blend-overlay"></div>
                <div className="max-w-4xl mx-auto px-4 relative z-10 text-center">
                    <h2 className="text-4xl font-bold text-white mb-6">Ready to transform your logistics?</h2>
                    <p className="text-xl text-slate-300 mb-10">Join thousands of transporters managing their fleet efficiently with Navgatix.</p>
                    <Link to="/register" className="bg-primary-500 hover:bg-primary-400 text-white font-semibold text-lg px-8 py-4 rounded-lg shadow-[0_0_20px_rgba(59,130,246,0.4)] transition-all inline-flex items-center gap-2">
                        Create an account
                        <ArrowRight className="h-5 w-5" />
                    </Link>
                </div>
            </section>

            {/* Contact Us */}
            <ContactUs />

            {/* Footer */}
            <footer className="bg-white border-t border-slate-200 py-12">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 flex flex-col md:flex-row justify-between items-center gap-6">
                    <div className="flex items-center gap-3">
                        <img src="/logo.png" alt="Navgatix Logo" className="h-12 object-contain" />
                        <span className="text-xl font-bold text-slate-900 tracking-tight">Navgatix</span>
                    </div>
                    <p className="text-slate-500">© 2026 Navgatix Platform. All rights reserved.</p>
                    <div className="flex space-x-6">
                        <a href="#" className="text-slate-400 hover:text-slate-600">Privacy</a>
                        <a href="#about" className="text-slate-400 hover:text-slate-600">Terms</a>
                        <a href="#contact" className="text-slate-400 hover:text-slate-600">Contact</a>
                    </div>
                </div>
            </footer>
        </div>
    );
};

export default LandingPage;
