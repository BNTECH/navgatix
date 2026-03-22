import { useState } from 'react';
import { Mail, Phone, MapPin, Send, Loader2 } from 'lucide-react';
import apiClient from '../api/apiClient';

const ContactUs = () => {
    const [formData, setFormData] = useState({
        name: '',
        emailId: '',
        phoneNumber: '',
        description: ''
    });
    const [status, setStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setStatus('loading');
        try {
            // Note: Replace with actual backend endpoint if different
            await apiClient.post('/User/contactSupport', {
                ...formData,
                createdDate: new Date().toISOString()
            });
            setStatus('success');
            setFormData({ name: '', emailId: '', phoneNumber: '', description: '' });
        } catch (error) {
            console.error('Submission error:', error);
            setStatus('error');
        }
    };

    return (
        <section id="contact" className="py-24 bg-white relative">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="grid md:grid-cols-2 gap-16 items-center">

                    {/* Contact Info */}
                    <div>
                        <h2 className="text-sm font-bold text-primary-600 tracking-wider uppercase mb-3">Get in Touch</h2>
                        <h3 className="text-4xl font-extrabold text-slate-900 mb-6">Let's Talk Logistics</h3>
                        <p className="text-lg text-slate-600 mb-10 leading-relaxed">
                            Have questions about our services or want to partner with us? Our team is available 24/7 to assist you.
                        </p>

                        <div className="space-y-8">
                            <div className="flex items-start gap-4">
                                <div className="w-12 h-12 bg-primary-50 rounded-xl flex items-center justify-center shrink-0">
                                    <Phone className="h-6 w-6 text-primary-600" />
                                </div>
                                <div>
                                    <h4 className="font-bold text-slate-900 mb-1">Call Us</h4>
                                    <p className="text-slate-600">+91 98765 43210</p>
                                    <p className="text-sm text-slate-500 mt-1">Available 24/7 for emergency support</p>
                                </div>
                            </div>

                            <div className="flex items-start gap-4">
                                <div className="w-12 h-12 bg-primary-50 rounded-xl flex items-center justify-center shrink-0">
                                    <Mail className="h-6 w-6 text-primary-600" />
                                </div>
                                <div>
                                    <h4 className="font-bold text-slate-900 mb-1">Email Us</h4>
                                    <p className="text-slate-600">support@navgatix.com</p>
                                    <p className="text-sm text-slate-500 mt-1">Drop us a line anytime!</p>
                                </div>
                            </div>

                            <div className="flex items-start gap-4">
                                <div className="w-12 h-12 bg-primary-50 rounded-xl flex items-center justify-center shrink-0">
                                    <MapPin className="h-6 w-6 text-primary-600" />
                                </div>
                                <div>
                                    <h4 className="font-bold text-slate-900 mb-1">Head Office</h4>
                                    <p className="text-slate-600">Block A, Tech Park, Chandigarh, 160001</p>
                                    <p className="text-sm text-slate-500 mt-1">Visit us (Appointment required)</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Contact Form */}
                    <div className="bg-slate-50 rounded-[2.5rem] p-8 md:p-12 border border-slate-200/60 shadow-xl shadow-slate-200/40">
                        <form onSubmit={handleSubmit} className="space-y-6">
                            <div>
                                <label htmlFor="name" className="block text-sm font-bold text-slate-700 mb-2">Full Name</label>
                                <input
                                    type="text"
                                    id="name"
                                    name="name"
                                    required
                                    value={formData.name}
                                    onChange={handleChange}
                                    className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 transition-all"
                                    placeholder="John Doe"
                                />
                            </div>
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                                <div>
                                    <label htmlFor="emailId" className="block text-sm font-bold text-slate-700 mb-2">Email Address</label>
                                    <input
                                        type="email"
                                        id="emailId"
                                        name="emailId"
                                        required
                                        value={formData.emailId}
                                        onChange={handleChange}
                                        className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 transition-all"
                                        placeholder="john@example.com"
                                    />
                                </div>
                                <div>
                                    <label htmlFor="phoneNumber" className="block text-sm font-bold text-slate-700 mb-2">Phone Number</label>
                                    <input
                                        type="tel"
                                        id="phoneNumber"
                                        name="phoneNumber"
                                        required
                                        value={formData.phoneNumber}
                                        onChange={handleChange}
                                        className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 transition-all"
                                        placeholder="+91 98765 43210"
                                    />
                                </div>
                            </div>
                            <div>
                                <label htmlFor="description" className="block text-sm font-bold text-slate-700 mb-2">Message</label>
                                <textarea
                                    id="description"
                                    name="description"
                                    required
                                    rows={4}
                                    value={formData.description}
                                    onChange={handleChange}
                                    className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 transition-all resize-none"
                                    placeholder="How can we help you?"
                                />
                            </div>

                            {status === 'success' && (
                                <div className="p-4 bg-emerald-50 text-emerald-700 rounded-xl text-sm font-medium border border-emerald-200">
                                    Message sent successfully! We will get back to you soon.
                                </div>
                            )}

                            {status === 'error' && (
                                <div className="p-4 bg-red-50 text-red-700 rounded-xl text-sm font-medium border border-red-200">
                                    Failed to send message. Please try again later.
                                </div>
                            )}

                            <button
                                type="submit"
                                disabled={status === 'loading'}
                                className="w-full btn-primary flex items-center justify-center gap-2 py-4 rounded-xl text-lg disabled:opacity-70"
                            >
                                {status === 'loading' ? (
                                    <>
                                        <Loader2 className="h-5 w-5 animate-spin" /> Sending...
                                    </>
                                ) : (
                                    <>
                                        Send Message <Send className="h-5 w-5" />
                                    </>
                                )}
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </section>
    );
};

export default ContactUs;
